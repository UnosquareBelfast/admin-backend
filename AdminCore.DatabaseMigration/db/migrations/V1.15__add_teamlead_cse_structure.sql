-- Alter Tables
----------------------------------------------------------------------------------------

/*
                                   EMPLOYEE ROLE TABLE
*/

----------------------------------------------------------------------------------------

CREATE SEQUENCE IF NOT EXISTS public.employee_role_employee_role_id_seq;
CREATE TABLE IF NOT EXISTS public.employee_role
(
  employee_role_id integer NOT NULL DEFAULT nextval('employee_role_employee_role_id_seq'::regclass),
  description character varying(255) COLLATE pg_catalog."default",
  CONSTRAINT employee_role_pkey PRIMARY KEY (employee_role_id)
)
  WITH (
    OIDS = FALSE
  )
  TABLESPACE pg_default;

ALTER SEQUENCE employee_role_employee_role_id_seq
  OWNED BY employee_role.employee_role_id;

INSERT INTO public.employee_role (employee_role_id, description)
VALUES (1, 'User'),
       (2, 'Team leader'),
       (3, 'CSE'),
       (4, 'Client')
ON CONFLICT (employee_role_id)
  DO NOTHING;

----------------------------------------------------------------------------------------

/*
                                   CONTRACT TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.contract
  ADD employee_role_id integer NOT NULL,

  ADD CONSTRAINT contract_employee_role_id_fkey FOREIGN KEY (employee_role_id)
    REFERENCES public.employee_role (employee_role_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

----------------------------------------------------------------------------------------

/*
                                   EVENT TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.event
  ADD team_id integer,

  ADD CONSTRAINT event_team_id_fkey FOREIGN KEY (team_id)
    REFERENCES public.team (team_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

----------------------------------------------------------------------------------------

/*
                                   PROJECT TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.event_type
  DROP CONSTRAINT project_project_parent_id_fkey,
  DROP COLUMN project_parent_id;

----------------------------------------------------------------------------------------

/*
                                   EVENT TYPE REQUIRED RESPONDERS TABLE
*/

----------------------------------------------------------------------------------------
DROP TABLE public.event_type_required_responders;

CREATE SEQUENCE IF NOT EXISTS public.event_type_required_responders_event_type_required_responders_id_seq;
CREATE TABLE IF NOT EXISTS public.event_type_required_responders
(
  -- Composite key declared in DBContext via fluent API.

  event_type_id integer NOT NULL,
  employee_role_id integer NOT NULL,

  CONSTRAINT "event_type_required_responders_pkey" PRIMARY KEY (event_type_id, employee_role_id),

  CONSTRAINT event_type_required_responders_event_type_id_fkey FOREIGN KEY (event_type_id)
    REFERENCES public.event_type (event_type_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION,
  CONSTRAINT event_type_required_responders_employee_role_id_fkey FOREIGN KEY (employee_role_id)
    REFERENCES public.employee_role (employee_role_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
)
  WITH (
    OIDS = FALSE
  )
  TABLESPACE pg_default;

ALTER SEQUENCE event_type_required_responders_event_type_required_responders_id_seq
  OWNED BY event_type_required_responders.event_type_id;

INSERT INTO public.event_type_required_responders (event_type_id, employee_role_id)
VALUES  (1, 2),
        (1, 3);
