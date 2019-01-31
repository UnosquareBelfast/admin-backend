INSERT INTO Employee(country_id, email, employee_role_id, employee_status_id, forename, password, start_date, surname, total_holidays, deleted)
VALUES (1, 'eoin.mcafee@unosquare.com', 2, 1, 'Eoin','', '2019-01-27','McAfee', 33, false);
INSERT INTO Employee(country_id, email, employee_role_id, employee_status_id, forename, password, start_date, surname, total_holidays, deleted)
VALUES (1, 'roisin.hughes@unosquare.com', 2, 1, 'Roisin','', '2019-01-27','Hughes', 33, false);
INSERT INTO Employee(country_id, email, employee_role_id, employee_status_id, forename, password, start_date, surname, total_holidays, deleted)
VALUES (1, 'katrina.jones@unosquare.com', 2, 1, 'Katrina','', '2019-01-27','Jones', 33, false);
INSERT INTO Employee(country_id, email, employee_role_id, employee_status_id, forename, password, start_date, surname, total_holidays, deleted)
VALUES (1, 'mark.brown@unosquare.com', 2, 1, 'Mark','', '2019-01-27','Brown', 33, false);

INSERT INTO Client (client_name, deleted) VALUES ('Unosquare', false);
INSERT INTO Client (client_name, deleted) VALUES ('Foundation Medicine', false);

INSERT INTO Team 	(	client_id, 																                        team_name, 					  contact_email, 						        contact_name,	  deleted	)
VALUES				    (	(SELECT client_id FROM Client WHERE client_name = 'Unosquare'), 	'Internal Project', 	'internalproject@unosquare.com',	'Eoin McAfee',	false	  );
INSERT INTO Team 	(	client_id, 																                                        team_name, 					  contact_email, 						              contact_name,	  deleted	)
VALUES				    (	(SELECT client_id FROM Client WHERE client_name = 'Foundation Medicine'), 				'Medical Billing 2', 'foundation.medicine@unosquare.com',			'Mark Brown',		false	  );


