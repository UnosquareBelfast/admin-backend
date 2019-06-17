-- Alter Tables
----------------------------------------------------------------------------------------

/*
                                   CONTRACT TABLE
*/

----------------------------------------------------------------------------------------
ALTER TABLE public.contract
ADD system_user_role_id integer,

ADD CONSTRAINT contract_system_user_role_id_fkey FOREIGN KEY (system_user_role_id)
  REFERENCES public.system_user_role (system_user_role_id) MATCH SIMPLE
  ON UPDATE NO ACTION
  ON DELETE NO ACTION;


----------------------------------------------------------------------------------------

/*
                                   TEAM TABLE
*/

  ----------------------------------------------------------------------------------------
ALTER TABLE public.team
  DROP COLUMN contact_email,
  DROP COLUMN contact_name;
