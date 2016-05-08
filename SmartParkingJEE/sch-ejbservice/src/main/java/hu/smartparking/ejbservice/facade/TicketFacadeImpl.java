package hu.smartparking.ejbservice.facade;

import hu.smartparking.ejbservice.converter.TicketConverter;
import hu.smartparking.ejbservice.domain.TicketStub;
import hu.smartparking.ejbservice.exception.AdaptorException;
import hu.smartparking.ejbservice.util.ApplicationError;
import hu.smartparking.persistence.Service.SensorService;
import hu.smartparking.persistence.Service.TicketService;
import hu.smartparking.persistence.entity.Sensor;
import hu.smartparking.persistence.exception.PersistenceServiceException;

import javax.ejb.EJB;
import javax.ejb.Stateless;
import java.util.Date;

/**
 * Created by Perec on 2016.04.17..
 */


@Stateless(mappedName = "ejb/ticketFacade")
public class TicketFacadeImpl implements TicketFacade{

    @EJB
    private TicketService ticketService;

    @EJB
    private SensorService sensorService;

    @EJB
    private TicketConverter ticketConverter;

    @Override
    public TicketStub addTicket(Long sensorId, Date start, Date end, String plate) throws AdaptorException {
        try{
            return this.ticketConverter.to(ticketService.create(sensorId, start, end, plate));
        } catch (final PersistenceServiceException e) {
            throw new AdaptorException(ApplicationError.UNEXPECTED, e.getLocalizedMessage());
        }
    }
}
