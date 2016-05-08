package hu.smartparking.webservice;

import hu.smartparking.ejbservice.domain.TicketInputStub;
import hu.smartparking.ejbservice.domain.TicketStub;
import hu.smartparking.ejbservice.exception.AdaptorException;
import hu.smartparking.ejbservice.facade.TicketFacade;

import javax.ejb.EJB;
import javax.ejb.Stateless;
import java.util.Date;

/**
 * Created by Perec on 2016.04.17..
 */

@Stateless
public class TicketRestServiceBean implements TicketRestService {

    @EJB
    private TicketFacade ticketFacade;

    @Override
    public TicketStub addTicket(TicketInputStub ticketInputStub) throws AdaptorException {
        return this.ticketFacade.addTicket(ticketInputStub.getSensor_id(),ticketInputStub.getStart(),ticketInputStub.getEnd(),ticketInputStub.getPlate());
    }
}
