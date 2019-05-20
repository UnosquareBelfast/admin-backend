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


----------------------------------------------------------------------------------------

/*
                                    TEAM TABLE
*/

----------------------------------------------------------------------------------------
-- CREATE INDEX team_team_id_idx ON public.team(team_id);

----------------------------------------------------------------------------------------

/*
                                    EVENT DATES TABLE
*/

----------------------------------------------------------------------------------------
-- CREATE INDEX event_dates_event_date_id_idx ON public.event_date(event_date_id);

----------------------------------------------------------------------------------------

/*
                                    EVENT TYPE TABLE
*/

----------------------------------------------------------------------------------------
-- CREATE INDEX event_type_event_type_id_idx ON public.event_type(event_type_id);

----------------------------------------------------------------------------------------

/*
                                    EMPLOYEE TABLE
*/

----------------------------------------------------------------------------------------
-- CREATE INDEX employee_employee_id_idx ON public.employee(employee_id);

----------------------------------------------------------------------------------------

/*
                                    EVENT STATUS TABLE
*/

----------------------------------------------------------------------------------------
-- CREATE INDEX event_status_event_status_id_idx ON public.event_status(event_status_id);

----------------------------------------------------------------------------------------

/*
                                    EVENT MESSAGE TABLE
*/

----------------------------------------------------------------------------------------
-- CREATE INDEX event_message_event_message_id_idx ON public.event_message(event_message_id);
