package com.unosquare.admin_core.back_end.entity;

import com.fasterxml.jackson.annotation.JsonIdentityInfo;
import com.fasterxml.jackson.annotation.ObjectIdGenerators;
import lombok.Data;

import javax.persistence.*;
import java.time.LocalDate;

@Entity
@Data
@JsonIdentityInfo(generator = ObjectIdGenerators.IntSequenceGenerator.class, property = "@id", scope = MandatoryHoliday.class)
@Table(name = "MandatoryHoliday")
public class MandatoryHoliday implements java.io.Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int mandatoryHolidayId;

    private LocalDate date;

    @Basic
    @Column(name = "country_id")
    private int countryId;

    public MandatoryHoliday() {

    }

    public MandatoryHoliday(LocalDate date, int countryId) {
        this.date = date;
        this.countryId = countryId;
    }
}
