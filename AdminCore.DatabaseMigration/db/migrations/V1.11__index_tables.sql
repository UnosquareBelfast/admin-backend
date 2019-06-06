/*
Description:
Adds indexing to specific tables that are used often/contain many data to help improve performance
*/

----------------------------------------------------------------------------------------

/*
                                    EVENT TABLE
*/

----------------------------------------------------------------------------------------
CREATE INDEX event_event_type_id_idx ON public.event(event_type_id);
CREATE INDEX event_employee_id_idx ON public.event(employee_id);

----------------------------------------------------------------------------------------

/*
                                    CONTRACT TABLE
*/

----------------------------------------------------------------------------------------
-- CREATE INDEX contract_employee_id_idx ON public.contract(employee_id);
CREATE INDEX contract_team_id_idx ON public.contract(team_id);
