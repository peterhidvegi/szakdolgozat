package hu.smartparking.persistence.Service;

import hu.smartparking.persistence.entity.Sensor;
import hu.smartparking.persistence.entity.Ticket;
import hu.smartparking.persistence.exception.PersistenceServiceException;

import javax.ejb.Local;
import java.sql.Timestamp;
import java.util.Date;

/**
 * Created by Perec on 2016.04.17..
 */

@Local
public interface TicketService {

    Ticket create(Long sensor_id, Date start, Date end, String plate) throws PersistenceServiceException;

}
