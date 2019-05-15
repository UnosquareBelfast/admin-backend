/*
Description:
Adds new tables required by the new workflow logic.
Adds new data to existing tables:
- EmployeeRole
-
*/

-- Add new schema

----------------------------------------------------------------------------------------

/*
                                   EVENT TYPE DAYS NOTICE TABLE
*/

----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.event_type_days_notice_event_type_days_notice_id_seq;
CREATE TABLE IF NOT EXISTS public.event_type_days_notice
(
  event_type_days_notice_id integer NOT NULL DEFAULT nextval('event_type_days_notice_event_type_days_notice_id_seq'::regclass),

  leave_length_days integer NOT NULL,
  days_notice integer NOT NULL,
  time_notice time,
  event_type_id integer NOT NULL,

  CONSTRAINT event_type_days_notice_pkey PRIMARY KEY (event_type_days_notice_id),

  CONSTRAINT event_type_days_notice_event_type_id_fkey FOREIGN KEY (event_type_id)
    REFERENCES public.event_type (event_type_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
)
  WITH (
    OIDS = FALSE
  )
  TABLESPACE pg_default;

ALTER SEQUENCE event_type_days_notice_event_type_days_notice_id_seq
  OWNED BY event_type_days_notice.event_type_days_notice_id;

----------------------------------------------------------------------------------------

/*
                                   EVENT WORKFLOW TABLE
*/

----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.event_workflow_event_workflow_id_seq;
CREATE TABLE IF NOT EXISTS public.event_workflow
(
  event_workflow_id integer NOT NULL DEFAULT nextval('event_workflow_event_workflow_id_seq'::regclass),

  workflow_state integer NOT NULL,

  CONSTRAINT event_workflow_pkey PRIMARY KEY (event_workflow_id)
)
  WITH (
    OIDS = FALSE
  )
  TABLESPACE pg_default;

ALTER SEQUENCE event_workflow_event_workflow_id_seq
  OWNED BY event_workflow.event_workflow_id;

----------------------------------------------------------------------------------------

/*
                                   EVENT TYPE REQUIRED RESPONDERS TABLE
*/

----------------------------------------------------------------------------------------
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
  response_sent_date date NOT NULL,
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

ALTER SEQUENCE employee_approval_response_employee_approval_response_id_seq
  OWNED BY employee_approval_response.employee_approval_response_id;

-- Alter existing tables

----------------------------------------------------------------------------------------

/*
                                   ALTER EVENT TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.event
  ADD event_workflow_id integer,

  ADD CONSTRAINT event_event_workflow_id_fkey FOREIGN KEY (event_workflow_id)
    REFERENCES public.event_workflow (event_workflow_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

-- Add new data

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

----------------------------------------------------------------------------------------

/*
                                   EVENT TYPE REQUIRED RESPONDERS TABLE
                                   Add the required responders for event types
*/

----------------------------------------------------------------------------------------
INSERT INTO public.event_type_required_responders (event_type_id, employee_role_id)
VALUES  (1, 1),
        (1, 4),
        (1, 5),
        (2, 1);

----------------------------------------------------------------------------------------

/*
                                   EVENT TYPE DAYS NOTICE TABLE
                                   Add the event type days notice required for leave.
*/

----------------------------------------------------------------------------------------
INSERT INTO public.event_type_days_notice (leave_length_days, days_notice, time_notice, event_type_id)
VALUES  (1, 7, null, 1),
        (4, 14, null, 1),
        (10, 60, null, 1),
        (1, 1, '16:00', 2)
  ON CONFLICT (event_type_days_notice_id)
    DO NOTHING;
