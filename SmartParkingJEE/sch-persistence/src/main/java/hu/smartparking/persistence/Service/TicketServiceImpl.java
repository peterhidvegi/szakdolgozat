package hu.smartparking.persistence.Service;

import hu.smartparking.persistence.entity.Sensor;
import hu.smartparking.persistence.entity.Ticket;
import hu.smartparking.persistence.exception.PersistenceServiceException;

import javax.ejb.*;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import java.sql.Timestamp;
import java.util.Date;

/**
 * Created by Perec on 2016.04.17..
 */

@Stateless(mappedName = "ejb/SensorService")
@TransactionManagement(TransactionManagementType.CONTAINER)
@TransactionAttribute(TransactionAttributeType.REQUIRES_NEW)
public class TicketServiceImpl implements TicketService {

    @PersistenceContext(unitName = "sch-persistence-unit")
    private EntityManager entityManager;

    @EJB
    private SensorService sensorService;

    @Override
    public Ticket create(Long sensor_id, Date start, Date end, String plate) throws PersistenceServiceException {

        Sensor sensor = this.sensorService.readById(sensor_id);
        Ticket newTicket = new Ticket(sensor, start, end, plate);
        entityManager.merge(newTicket);
        this.entityManager.flush();
        return newTicket;
    }

}
