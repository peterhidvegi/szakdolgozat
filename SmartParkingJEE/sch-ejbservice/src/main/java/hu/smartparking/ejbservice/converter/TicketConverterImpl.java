package hu.smartparking.ejbservice.converter;

import hu.smartparking.ejbservice.domain.SensorStub;
import hu.smartparking.ejbservice.domain.StatusStub;
import hu.smartparking.ejbservice.domain.TicketStub;
import hu.smartparking.persistence.entity.Ticket;

import javax.ejb.EJB;
import javax.ejb.Stateless;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Perec on 2016.04.17..
 */
@Stateless
public class TicketConverterImpl implements TicketConverter {

    @EJB
    private SensorConverter sensorConverter;

    @Override
    public TicketStub to(Ticket ticket) {

        return new TicketStub(this.sensorConverter.to(ticket.getSensor()),ticket.getStart(),ticket.getEnd(),ticket.getPlate());
    }


    @Override
    public List<TicketStub> to(List<Ticket> tickets) {
        List<TicketStub> ticketStubs = new ArrayList<>();
        for(final Ticket ticket : tickets)
        {
            ticketStubs.add(this.to(ticket));
        }

        return ticketStubs;
    }
}
