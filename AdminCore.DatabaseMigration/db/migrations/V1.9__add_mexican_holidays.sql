INSERT INTO public.mexican_holiday (mexican_holiday_id, years_with_company, entitled_holidays)
VALUES (0, 0, 6),
			 (1, 1, 8),
       (2, 2, 10),
       (3, 3, 12),
			 (4, 4, 14),
       (5, 5, 16),
       (6, 6, 16),
       (7, 7, 16),
       (8, 8, 16),
       (9, 9, 16),
       (10, 10, 18),
       (11, 11, 18),
       (12, 12, 18),
			 (13, 13, 18),
       (14, 14, 18)
ON CONFLICT (mexican_holiday_id)
  DO NOTHING;