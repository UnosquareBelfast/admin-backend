/*
Description:
Adds new tables required by the new workflow logic.
Adds new data to existing tables:
- EmployeeRole
- 
*/
-- Alter existing tables

----------------------------------------------------------------------------------------

/*
                                   ALTER EVENT TABLE
*/

----------------------------------------------------------------------------------------
-- ALTER TABLE public.event
--   ADD event_workflow_id integer NOT NULL;
  
-- Add new schema

----------------------------------------------------------------------------------------

/*
                                   EVENT WORKFLOW TABLE
*/

----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.event_workflow_event_workflow_id_seq;
CREATE TABLE IF NOT EXISTS public.event_workflow
(
  event_workflow_id integer NOT NULL DEFAULT nextval('event_workflow_event_workflow_id_seq'::regclass),

  event_id integer NOT NULL,
  workflow_state integer NOT NULL,

  CONSTRAINT event_workflow_pkey PRIMARY KEY (event_workflow_id),

  CONSTRAINT event_workflow_event_id_fkey FOREIGN KEY (event_id)
    REFERENCES public.event (event_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
)
  WITH (
    OIDS = FALSE
  )
  TABLESPACE pg_default;

ALTER SEQUENCE employee_employee_id_seq
  OWNED BY employee.employee_id;
  
----------------------------------------------------------------------------------------

/*
                                   EVENT WORKFLOW RESPONDER TABLE
*/

----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.event_workflow_responder_event_workflow_responder_id_seq;
CREATE TABLE IF NOT EXISTS public.event_workflow_responder
(
  event_workflow_responder_id integer NOT NULL DEFAULT nextval('event_workflow_responder_event_workflow_responder_id_seq'::regclass),

  event_workflow_id integer NOT NULL,
  employee_role_id integer NOT NULL,
  
  CONSTRAINT "event_workflow_responder_pkey" PRIMARY KEY (event_workflow_id, employee_role_id),
  
  CONSTRAINT event_workflow_responder_event_workflow_id_fkey FOREIGN KEY (event_workflow_id)
    REFERENCES public.event_workflow (event_workflow_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION,
  CONSTRAINT event_workflow_responder_employee_role_id_fkey FOREIGN KEY (employee_role_id)
    REFERENCES public.employee_role (employee_role_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
)
  WITH (
    OIDS = FALSE
  )
  TABLESPACE pg_default;

----------------------------------------------------------------------------------------

/*
                                   EMPLOYEE APPROVAL RESPONSE TABLE
*/

----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.employee_approval_response_employee_approval_response_id_seq;
CREATE TABLE IF NOT EXISTS public.employee_approval_response
(
  employee_approval_response_id integer NOT NULL DEFAULT nextval('employee_approval_response_employee_approval_response_id_seq'::regclass),

  employee_role_id integer NOT NULL,
  event_status_id integer NOT NULL,
  event_workflow_id integer NOT NULL,
  employee_id integer NOT NULL,

  CONSTRAINT employee_approval_response_pkey PRIMARY KEY ("employee_approval_response_id"),

  CONSTRAINT employee_approval_response_employee_role_id_fkey FOREIGN KEY (employee_role_id)
    REFERENCES public.employee_role (employee_role_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION,
  CONSTRAINT employee_approval_response_event_status_id_fkey FOREIGN KEY (event_status_id)
    REFERENCES public.event_status (event_status_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION,
  CONSTRAINT employee_approval_response_event_workflow_id_fkey FOREIGN KEY (event_workflow_id)
    REFERENCES public.event_workflow (event_workflow_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION,
  CONSTRAINT employee_approval_response_employee_id_fkey FOREIGN KEY (employee_id)
    REFERENCES public.employee (employee_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
)
  WITH (
    OIDS = FALSE
  )
  TABLESPACE pg_default;
  
-- Add new Data
----------------------------------------------------------------------------------------

/*
                                   EMPLOYEE ROLE TABLE
                                   Add new employee roles
*/

----------------------------------------------------------------------------------------
INSERT INTO public.employee_role (employee_role_id, description)
VALUES (4, 'Cse'),
       (5, 'Client')
ON CONFLICT (employee_role_id)
             DO NOTHING;

