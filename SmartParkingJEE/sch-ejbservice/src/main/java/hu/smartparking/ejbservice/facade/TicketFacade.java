package hu.smartparking.ejbservice.facade;


import hu.smartparking.ejbservice.domain.TicketStub;
import hu.smartparking.ejbservice.exception.AdaptorException;
import hu.smartparking.persistence.entity.Sensor;

import javax.ejb.Local;
import java.util.Date;

/**
 * Created by Perec on 2016.04.17..
 */

@Local
public interface TicketFacade {

    TicketStub addTicket(Long sensorId,Date start, Date end, String plate) throws AdaptorException;

}
