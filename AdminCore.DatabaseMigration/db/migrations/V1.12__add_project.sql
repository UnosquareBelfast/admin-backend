-- Add tables
  ----------------------------------------------------------------------------------------

/*
                                   PROJECT TABLE
*/

  ----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.project_project_id_seq;
CREATE TABLE IF NOT EXISTS public.project
(
    project_id integer NOT NULL DEFAULT nextval('project_project_id_seq'::regclass),
    project_parent_id integer,
    project_name character varying(255) COLLATE pg_catalog."default",
    client_id integer NOT NULL,
    deleted boolean NOT NULL,

    CONSTRAINT project_pkey PRIMARY KEY (project_id),

    CONSTRAINT project_project_parent_id_fkey FOREIGN KEY (project_parent_id)
      REFERENCES public.project (project_id) MATCH SIMPLE
      ON UPDATE NO ACTION
      ON DELETE NO ACTION,
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
INSERT INTO public.project (project_name, client_id, deleted)
SELECT team_name, client_id, false FROM public.team;

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
    ON DELETE NO ACTION;

-- Set the project_id column on team to the project id that matches the old team => client relationship.
-- Ensure that there is a default project created for every existing team/client and the foreign keys are assigned correctly before dropping data.
UPDATE public.team
SET
  project_id = project.project_id
FROM
  project
WHERE
  team.client_id = project.client_id;

-- Drop columns
----------------------------------------------------------------------------------------

/*
                                   TEAM TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.team
DROP COLUMN client_id,
ALTER COLUMN project_id SET NOT NULL;
