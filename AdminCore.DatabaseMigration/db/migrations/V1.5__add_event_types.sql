INSERT INTO public.event_type (event_type_id, description)
VALUES (0, 'Public Holiday'),
			 (1, 'Annual Leave'),
       (2, 'Working From Home'),
       (3, 'Bereavement'),
       (4, 'Jury Service'),
			 (5, 'On Business'),
			 (6, 'Parental Leave'),
			 (7, 'Shared Parental Leave'),
       (8, 'Maternity Leave'),
			 (9, 'Paternity Leave'),
			 (10, 'Adoption Leave'),
			 (11, 'Medical Leave'),
       (12, 'Sick Leave'),
			 (13, 'Training'),
			 (14, 'Unauthorised Leave'),
			 (15, 'Wedding'),
       (16, 'Compassionate'),
			 (17, 'Emergency Leave For Dependance'),
			 (18, 'Moving House')
ON CONFLICT (event_type_id)
  DO NOTHING;