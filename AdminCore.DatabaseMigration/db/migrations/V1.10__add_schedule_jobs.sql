/*
                                   SCHEDULE JOB TABLE
*/
  ----------------------------------------------------------------------------------------
CREATE SEQUENCE IF NOT EXISTS public.schedule_job_schedule_job_id_seq;
CREATE TABLE IF NOT EXISTS public.schedule_job
(
    schedule_job_id integer NOT NULL DEFAULT nextval('schedule_job_schedule_job_id_seq'::regclass),
    schedule_job_name text NOT NULL,
		schedule_cron_expression text NOT NULL,
    is_active boolean NOT NULL,
    CONSTRAINT schedule_job_pkey PRIMARY KEY (schedule_job_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER SEQUENCE schedule_job_schedule_job_id_seq
    OWNED BY schedule_job.schedule_job_id;

INSERT INTO public.schedule_job (schedule_job_id, schedule_job_name, schedule_cron_expression, is_active)
VALUES (1, 'Prune Expired Events Job', '0 0 3 ? * *', true)
ON CONFLICT (schedule_job_id)
  DO NOTHING;