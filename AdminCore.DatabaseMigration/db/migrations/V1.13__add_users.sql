-- Add new schema

----------------------------------------------------------------------------------------

/*
                                   SYSTEM USER ROLE TABLE
*/

----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.system_user_role_system_user_role_id_seq;
CREATE TABLE IF NOT EXISTS public.system_user_role
(
  system_user_role_id integer NOT NULL DEFAULT nextval('system_user_role_system_user_role_id_seq'::regclass),

  description character varying(255) COLLATE pg_catalog."default",

  CONSTRAINT system_user_role_pkey PRIMARY KEY (system_user_role_id)
)
  WITH (
    OIDS = FALSE
  )
  TABLESPACE pg_default;

ALTER SEQUENCE system_user_role_system_user_role_id_seq
  OWNED BY system_user_role.system_user_role_id;

----------------------------------------------------------------------------------------

/*
                                   SYSTEM USER TABLE
*/

----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.system_user_system_user_id_seq;
CREATE TABLE IF NOT EXISTS public.system_user
(
  system_user_id integer NOT NULL DEFAULT nextval('system_user_system_user_id_seq'::regclass),

  system_user_role_id integer NOT NULL,
  deleted        boolean NOT NULL,

  temporary_id_store integer NOT NULL,

  CONSTRAINT system_user_pkey PRIMARY KEY (system_user_id),

  CONSTRAINT system_user_system_user_role_id_fkey FOREIGN KEY (system_user_role_id)
  REFERENCES public.system_user_role (system_user_role_id) MATCH SIMPLE
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
                                   EMPLOYEE TABLE
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
                                   CLIENT TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.client
  ADD system_user_id integer,

  ADD CONSTRAINT client_system_user_id_fkey FOREIGN KEY (system_user_id)
    REFERENCES public.system_user (system_user_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

----------------------------------------------------------------------------------------

/*
                                   EVENT MESSAGE TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.event_message
  ADD system_user_id integer,

  ADD CONSTRAINT event_message_system_user_id_fkey FOREIGN KEY (system_user_id)
    REFERENCES public.system_user (system_user_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

----------------------------------------------------------------------------------------

/*
                                   EVENT TYPE TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.event_type
  ADD system_user_role_id integer,

  ADD CONSTRAINT event_type_system_user_role_id_fkey FOREIGN KEY (system_user_role_id)
    REFERENCES public.system_user_role (system_user_role_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

-- Add new data
----------------------------------------------------------------------------------------

/*
                                   SYSTEM USER ROLE TABLE
                                   Add new system user type entries
*/

----------------------------------------------------------------------------------------
INSERT INTO public.system_user_role(description)
SELECT description FROM public.employee_role;

INSERT INTO public.system_user_role (system_user_role_id, description)
VALUES (4, 'Cse'),
       (5, 'Client')
ON CONFLICT (system_user_role_id)
  DO NOTHING;

-- Perform data update on tables
----------------------------------------------------------------------------------------

/*
                                   EMPLOYEE/CLIENT TABLE
    Add new users for every employee/client and update foreign keys on respective parent tables.
*/

----------------------------------------------------------------------------------------
-- Employee
with new_rows as
       (
         INSERT INTO public.system_user (system_user_role_id, temporary_id_store, deleted)
           SELECT employee_role_id, employee_id, false FROM public.employee
           RETURNING *
       )
UPDATE public.employee
SET
  system_user_id = new_rows.system_user_id
FROM
  new_rows
WHERE
  new_rows.temporary_id_store = employee.employee_id;

-- Client
with new_rows as
       (
         INSERT INTO public.system_user (system_user_role_id, temporary_id_store, deleted)
           SELECT 5, client_id, false FROM public.client
           RETURNING *
       )
UPDATE public.client
SET
  system_user_id = new_rows.system_user_id
FROM
  new_rows
WHERE
  new_rows.temporary_id_store = client.client_id;

----------------------------------------------------------------------------------------

/*
                                   EVENT MESSAGE TABLE
*/

----------------------------------------------------------------------------------------
UPDATE public.event_message
SET
  system_user_id = employee.system_user_id
FROM
  employee
WHERE
  employee.employee_id = event_message.employee_id;

----------------------------------------------------------------------------------------

/*
                                   EVENT TYPE TABLE
*/

----------------------------------------------------------------------------------------
UPDATE public.event_type
SET
  system_user_role_id = system_user_role.system_user_role_id
FROM
  system_user_role
WHERE
    event_type.employee_role_id = system_user_role.system_user_role_id;

-- Drop redundant columns/foreign keys
----------------------------------------------------------------------------------------

/*
                                   SYSTEM USER TABLE
                                   Drop temporary id store
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.system_user
  DROP COLUMN temporary_id_store;

----------------------------------------------------------------------------------------

/*
                                   EMPLOYEE TABLE
                                   Add new constraint
                           Drop EmployeeRole column and constraint to Employee Role
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.employee
  ADD CONSTRAINT employee_system_user_id_constr UNIQUE (system_user_id),

  DROP CONSTRAINT employee_employee_role_id_fkey,
  DROP COLUMN employee_role_id;

----------------------------------------------------------------------------------------

/*
                                   CLIENT TABLE
                                   Add new constraint
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.client
  ADD CONSTRAINT client_system_user_id_constr UNIQUE (system_user_id);

----------------------------------------------------------------------------------------

/*
                                   EVENT MESSAGE TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.event_message
  DROP CONSTRAINT event_message_employee_id_fkey,
  DROP COLUMN employee_id;

----------------------------------------------------------------------------------------

/*
                                   EVENT TYPE TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.event_type
  DROP CONSTRAINT employee_employee_role_id_fkey,
  DROP COLUMN employee_role_id;

----------------------------------------------------------------------------------------

/*
                                   EMPLOYEE ROLE TABLE
*/

----------------------------------------------------------------------------------------
DROP TABLE public.employee_role;
