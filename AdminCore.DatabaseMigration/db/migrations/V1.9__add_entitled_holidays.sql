INSERT INTO public.entitled_holiday (entitled_holiday_id, country_id, month, years_with_company, entitled_holidays)
VALUES (1, 1, 1, null, 30),
       (2, 1, 2, null, 28),
       (3, 1, 3, null, 25),
			 (4, 1, 4, null, 23),
       (5, 1, 5, null, 20),
       (6, 1, 6, null, 18),
       (7, 1, 7, null, 15),
       (8, 1, 8, null, 13),
       (9, 1, 9, null, 10),
       (10, 1, 10, null, 8),
       (11, 1, 11, null, 5),
       (12, 1, 12, null, 3),
			 (13, 2, null, 0, 6),
			 (14, 2, null, 1, 8),
       (15, 2, null, 2, 10),
       (16, 2, null, 3, 12),
			 (17, 2, null, 4, 14),
       (18, 2, null, 5, 16),
       (19, 2, null, 6, 16),
       (20, 2, null, 7, 16),
       (21, 2, null, 8, 16),
       (22, 2, null, 9, 16),
       (23, 2, null, 10, 18),
       (24, 2, null, 11, 18),
       (25, 2, null, 12, 18),
			 (26, 2, null, 13, 18),
       (27, 2, null, 14, 18)
ON CONFLICT (entitled_holiday_id)
  DO NOTHING;