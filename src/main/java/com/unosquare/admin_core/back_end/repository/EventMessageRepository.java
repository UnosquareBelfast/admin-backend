package com.unosquare.admin_core.back_end.repository;

import com.unosquare.admin_core.back_end.entity.EventMessage;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface EventMessageRepository extends JpaRepository<EventMessage, Integer> {
    @Query(value = "SELECT em FROM EventMessage em " +
            "WHERE " +
            "em.event.eventId = :eventId"
    )
    List<EventMessage> findEventMessagesByEventId(@Param("eventId") int eventId);
}
