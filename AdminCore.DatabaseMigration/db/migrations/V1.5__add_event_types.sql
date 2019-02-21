INSERT INTO public.event_type (event_type_id, description, employee_role_id)
VALUES (0, 'Public Holiday', 2),
			 (1, 'Annual Leave', 3),
       (2, 'Working From Home', 3),
       (3, 'Bereavement', 3),
       (4, 'Jury Service', 3),
			 (5, 'On Business', 3),
			 (6, 'Parental Leave', 2),
			 (7, 'Shared Parental Leave', 2),
       (8, 'Maternity Leave', 2),
			 (9, 'Paternity Leave', 2),
			 (10, 'Adoption Leave', 2),
			 (11, 'Medical Leave', 3),
       (12, 'Sick Leave', 3),
			 (13, 'Training', 3),
			 (14, 'Unauthorised Leave', 3),
			 (15, 'Wedding', 3),
       (16, 'Compassionate', 2),
			 (17, 'Emergency Leave For Dependance', 2),
			 (18, 'Moving House', 2)
ON CONFLICT (event_type_id)
  DO NOTHING;