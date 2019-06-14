 ----------------------------------------------------------------------------------------

/*
                             EVENT REQUEST RESPONSE TYPE TABLE
*/

  ----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.event_request_response_type_request_response_type_id_seq;
CREATE TABLE IF NOT EXISTS public.event_request_response_type
(
    request_response_type_id integer NOT NULL,
    type_value character varying(25) NOT NULL,
    CONSTRAINT request_response_type_request_response_type_id_pkey PRIMARY KEY (request_response_type_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER SEQUENCE event_request_response_type_request_response_type_id_seq
    OWNED BY event_request_response_type.request_response_type_id;

INSERT INTO public.event_request_response_type (request_response_type_id, type_value)
VALUES (0, 'Accept'),
       (1, 'Reject'),
       (2, 'Cancel')

ON CONFLICT (request_response_type_id)
  DO NOTHING;


  ----------------------------------------------------------------------------------------

/*
                                  EVENT REQUEST STATUS TABLE
*/

  ----------------------------------------------------------------------------------------sZ
CREATE SEQUENCE IF NOT EXISTS public.event_request_status_request_status_id_seq;
CREATE TABLE IF NOT EXISTS public.event_request_status
(
    request_status_id integer NOT NULL,
    type_value character varying(25) NOT NULL,
    CONSTRAINT request_status_request_status_id_pkey PRIMARY KEY (request_status_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER SEQUENCE event_request_status_request_status_id_seq
    OWNED BY event_request_status.request_status_id;

INSERT INTO public.event_request_status (request_status_id, type_value)
VALUES (0, 'Accept'),
       (1, 'Reject'),
       (2, 'Pending')

ON CONFLICT (request_status_id)
  DO NOTHING;


  ----------------------------------------------------------------------------------------

/*
                                  EVENT REQUEST TYPE TABLE
*/

  ----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.event_request_type_request_type_id_seq;
CREATE TABLE IF NOT EXISTS public.event_request_type
(
    request_type_id integer NOT NULL,
    type_value character varying(25) NOT NULL,
    life_span integer NULL,
    locked boolean NOT NULL,
    CONSTRAINT request_type_request_type_id_pkey PRIMARY KEY (request_type_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER SEQUENCE event_request_type_request_type_id_seq
    OWNED BY event_request_type.request_type_id;

INSERT INTO public.event_request_type (request_type_id, type_value, life_span, locked)
VALUES (1, 'Client', 48, true),
       (2, 'HR', 48, true),
       (3, 'Team Lead', 48, false),
       (4, 'CSE', null, false),
       (5, 'Cancellation Notice', null, false)

ON CONFLICT (request_type_id)
  DO NOTHING;


  ----------------------------------------------------------------------------------------

/*
                                   EVENT REQUEST TABLE
*/

  ----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.event_request_event_request_id_seq;
CREATE TABLE IF NOT EXISTS public.event_request
(
    event_request_id integer NOT NULL DEFAULT nextval('public.event_request_event_request_id_seq'),
    request_response_type_id integer NOT NULL,
    request_type_id integer NOT NULL,
    request_status_id integer NOT NULL,
    salt character varying(255) NOT NULL,
    hash character varying(255) NOT NULL,
    CONSTRAINT event_request_event_request_id_pkey PRIMARY KEY (event_request_id),
    CONSTRAINT event_request_response_type_request_response_type_id_fkey FOREIGN KEY (request_response_type_id)
        REFERENCES public.event_request_response_type (request_response_type_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT event_request_type_request_type_id_fkey FOREIGN KEY (request_type_id)
        REFERENCES public.event_request_type (request_type_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT event_request_status_request_status_id_fkey FOREIGN KEY (request_status_id)
        REFERENCES public.event_request_status (request_status_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER SEQUENCE event_request_event_request_id_seq
    OWNED BY event_request.event_request_id;


  ----------------------------------------------------------------------------------------

/*
                                   EVENT REQUEST STATE TABLE
*/

  ----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.event_request_state_request_state_id_seq;
CREATE TABLE IF NOT EXISTS public.event_request_state
(
    request_state_id integer NOT NULL DEFAULT nextval('public.event_request_state_request_state_id_seq'),
    event_id integer NOT NULL,
    system_user_id integer NOT NULL,
    event_request_id integer NOT NULL,
    time_expires timestamp NOT NULL,
    active boolean NOT NULL,
    auto_approved boolean NOT NULL,
    CONSTRAINT event_request_state_request_state_id_pkey PRIMARY KEY (request_state_id),
    CONSTRAINT event_request_state_event_id_fkey FOREIGN KEY (event_id)
        REFERENCES public.event (event_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT event_request_state_system_user_id_fkey FOREIGN KEY (system_user_id)
        REFERENCES public.system_user (system_user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT event_request_state_event_request_id_fkey FOREIGN KEY (event_request_id)
        REFERENCES public.event_request (event_request_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER SEQUENCE event_request_state_request_state_id_seq
    OWNED BY event_request_state.request_state_id;
