INSERT INTO public.northern_irish_holiday (northern_irish_holiday_id, month, entitled_holidays)
VALUES (1, 1, 30),
       (2, 2, 28),
       (3, 3, 25),
			 (4, 4, 23),
       (5, 5, 20),
       (6, 6, 18),
       (7, 7, 15),
       (8, 8, 13),
       (9, 9, 10),
       (10, 10, 8),
       (11, 11, 5),
       (12, 12, 3)
ON CONFLICT (northern_irish_holiday_id)
  DO NOTHING;