-- Add tables
  ----------------------------------------------------------------------------------------

/*
                                   EMPLOYEE TABLE
*/

  ----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.project_project_id_seq;
CREATE TABLE IF NOT EXISTS public.project
(
    project_id integer NOT NULL DEFAULT nextval('project_project_id_seq'::regclass),
    project_name character varying(255) COLLATE pg_catalog."default",
    client_id integer NOT NULL,
    deleted boolean NOT NULL,

    CONSTRAINT project_pkey PRIMARY KEY (project_id),

    CONSTRAINT project_client_id_fkey FOREIGN KEY (client_id)
        REFERENCES public.client (client_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER SEQUENCE employee_employee_id_seq
    OWNED BY employee.employee_id;

-- Add initial data and migrate data
----------------------------------------------------------------------------------------

/*
                                   PROJECT TABLE
*/

----------------------------------------------------------------------------------------

-- Alter existing tables
----------------------------------------------------------------------------------------

/*
                                   TEAM TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.team
  ADD project_id integer,

  ADD CONSTRAINT team_project_id_fkey FOREIGN KEY (project_id)
    REFERENCES public.project (project_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION,

  DROP COLUMN client_id;
