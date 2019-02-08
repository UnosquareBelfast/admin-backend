INSERT INTO public.public_holiday (public_holiday_id, country_id, public_holiday_date)
VALUES (1, 1, '2019-01-01'),
       (2, 1, '2019-05-27'),
       (3, 1, '2019-12-25')
ON CONFLICT (public_holiday_id)
  DO NOTHING;