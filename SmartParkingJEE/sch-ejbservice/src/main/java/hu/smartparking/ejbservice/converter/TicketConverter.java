package hu.smartparking.ejbservice.converter;

import hu.smartparking.ejbservice.domain.TicketStub;
import hu.smartparking.persistence.entity.Ticket;

import javax.ejb.Local;
import java.util.List;

/**
 * Created by Perec on 2016.04.17..
 */
@Local
public interface TicketConverter {

    TicketStub to(Ticket ticket);

    List<TicketStub> to(List<Ticket> tickets);
}
