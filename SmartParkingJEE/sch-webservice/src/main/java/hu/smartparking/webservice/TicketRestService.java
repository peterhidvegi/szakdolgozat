package hu.smartparking.webservice;

import hu.smartparking.ejbservice.domain.TicketInputStub;
import hu.smartparking.ejbservice.domain.TicketStub;
import hu.smartparking.ejbservice.exception.AdaptorException;

import javax.ws.rs.Consumes;
import javax.ws.rs.PUT;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import java.util.Date;

/**
 * Created by Perec on 2016.04.17..
 */

@Path("/ticket")
public interface TicketRestService{

    @PUT
    @Path("/add")
    @Consumes("application/json")
    @Produces("application/json")
    TicketStub addTicket(TicketInputStub ticketInputStub) throws AdaptorException;

}
