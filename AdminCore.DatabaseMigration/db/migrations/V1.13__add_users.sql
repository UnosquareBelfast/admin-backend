-- Add new schema

----------------------------------------------------------------------------------------

/*
                                   SYSTEM USER TYPE TABLE
*/

----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.system_user_type_system_user_type_id_seq;
CREATE TABLE IF NOT EXISTS public.system_user_type
(
  system_user_type_id integer NOT NULL DEFAULT nextval('system_user_type_system_user_type_id_seq'::regclass),

  description character varying(255) COLLATE pg_catalog."default",

  CONSTRAINT system_user_type_pkey PRIMARY KEY (system_user_type_id)
)
  WITH (
    OIDS = FALSE
  )
  TABLESPACE pg_default;

ALTER SEQUENCE system_user_type_system_user_type_id_seq
  OWNED BY system_user_type.system_user_type_id;

----------------------------------------------------------------------------------------

/*
                                   SYSTEM USER TABLE
*/

----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.system_user_system_user_id_seq;
CREATE TABLE IF NOT EXISTS public.system_user
(
  system_user_id integer NOT NULL DEFAULT nextval('system_user_system_user_id_seq'::regclass),

  system_user_type_id integer NOT NULL,
  deleted        boolean NOT NULL,

  system_user_temporary_id_store integer NOT NULL,

  CONSTRAINT system_user_pkey PRIMARY KEY (system_user_id),

  CONSTRAINT system_user_system_user_type_id_fkey FOREIGN KEY (system_user_type_id)
  REFERENCES public.system_user_type (system_user_type_id) MATCH SIMPLE
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
)
  WITH (
    OIDS = FALSE
  )
  TABLESPACE pg_default;

ALTER SEQUENCE system_user_system_user_id_seq
  OWNED BY system_user.system_user_id;

-- Alter existing tables

----------------------------------------------------------------------------------------

/*
                                   ALTER EMPLOYEE TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.employee

  ADD system_user_id integer,

  ADD CONSTRAINT employee_system_user_id_fkey FOREIGN KEY (system_user_id)
    REFERENCES public.system_user (system_user_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

----------------------------------------------------------------------------------------

/*
                                   ALTER CLIENT TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.client
  ADD system_user_id integer,

  ADD CONSTRAINT client_system_user_id_fkey FOREIGN KEY (system_user_id)
    REFERENCES public.system_user (system_user_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


-- Add new data
----------------------------------------------------------------------------------------

/*
                                   SYSTEM USER TYPE TABLE
                                   Add new system user type entries
*/

----------------------------------------------------------------------------------------
INSERT INTO public.system_user_type (system_user_type_id, description)
  VALUES (1, 'Employee'),
        (2, 'Client')
  ON CONFLICT (system_user_type_id)
    DO NOTHING;


-- Alter tables

----------------------------------------------------------------------------------------

/*
                                   SYSTEM USER TABLE
                                   Add new users for every employee/client and update foreign keys on respective parent tables.
*/

----------------------------------------------------------------------------------------
with new_rows as
       (
         INSERT INTO public.system_user (system_user_type_id, deleted, system_user_temporary_id_store)
           SELECT 1, false, employee_id FROM public.employee
           RETURNING *
       )
UPDATE public.employee
SET
  system_user_id = new_rows.system_user_id
FROM
  new_rows
WHERE
    new_rows.system_user_temporary_id_store = employee.employee_id;

with new_rows as
       (
         INSERT INTO public.system_user (system_user_type_id, deleted, system_user_temporary_id_store)
           SELECT 2, false, client_id FROM public.client
           RETURNING *
       )
UPDATE public.client
SET
  system_user_id = new_rows.system_user_id
FROM
  new_rows
WHERE
    new_rows.system_user_temporary_id_store = client.client_id;

----------------------------------------------------------------------------------------

/*
                                   SYSTEM USER TABLE
                                   Drop temporary id store
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.system_user
  DROP COLUMN system_user_temporary_id_store;

----------------------------------------------------------------------------------------

/*
                                   EMPLOYEE TABLE
                                   Alter column to be not null
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.employee
  ADD CONSTRAINT employee_system_user_id_constr UNIQUE (system_user_id);

----------------------------------------------------------------------------------------

/*
                                   CLIENT TABLE
                                   Alter column to be not null
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.client
  ADD CONSTRAINT client_system_user_id_constr UNIQUE (system_user_id);
