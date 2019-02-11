INSERT INTO public.public_holiday (public_holiday_id, country_id, public_holiday_date)
VALUES (1, 1, '2019-01-01'),
       (2, 1, '2019-05-27'),
       (3, 1, '2019-12-25'),
			 (4, 2, '2019-01-01'),
       (5, 2, '2019-05-27'),
			 (6, 2, '2019-07-04'),
       (7, 2, '2019-09-02'),
			 (8, 2, '2019-09-16'),
       (9, 2, '2019-11-28'),
			 (10, 2, '2019-11-29'),
			 (11, 2, '2019-12-25')
ON CONFLICT (public_holiday_id)
  DO NOTHING;