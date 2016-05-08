package hu.smartparking.persistence.Service;

import hu.smartparking.persistence.entity.Sensor;
import hu.smartparking.persistence.enums.Status;
import hu.smartparking.persistence.exception.PersistenceServiceException;
import hu.smartparking.persistence.parameter.SensorParameter;
import hu.smartparking.persistence.query.SensorQuery;

import javax.ejb.*;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import java.util.List;

/**
 * Created by Perec on 2016.04.10..
 */

@Stateless(mappedName = "ejb/SensorService")
@TransactionManagement(TransactionManagementType.CONTAINER)
@TransactionAttribute(TransactionAttributeType.REQUIRES_NEW)
public class SensorServiceImpl implements SensorService {

    @PersistenceContext(unitName = "sch-persistence-unit")
    private EntityManager entityManager;


    @Override
    public Sensor readById(Long sensorId) throws PersistenceServiceException {
        Sensor result = null;
        try{
            result = this.entityManager.createNamedQuery(SensorQuery.GET_BY_ID, Sensor.class).setParameter(SensorParameter.ID,sensorId).getSingleResult();
        }catch (final Exception e)
        {
            throw new PersistenceServiceException("Unknown error when fetching Sensor by ID "+sensorId+". Error: "+e.getLocalizedMessage(), e);
        }
        return result;
    }
/*
    @Override
    public List<Sensor> readByRdy(Status status) throws PersistenceServiceException {

        List<Sensor> result = null;
        try{
            result = this.entityManager.createNamedQuery(SensorQuery.GET_RDY, Sensor.class).setParameter(SensorParameter.STATUS,status).getResultList();
        }catch (final Exception e)
        {
            throw new PersistenceServiceException("Unknown error when fetching Sensor by status "+status+". Error: "+e.getLocalizedMessage(), e);
        }
        return result;
    }
*/
    @Override
    public List<Sensor> readAll() throws PersistenceServiceException {
        List<Sensor> result = null;
        try{
            result = this.entityManager.createNamedQuery(SensorQuery.GET_ALL, Sensor.class).getResultList();
        }catch (final Exception e)
        {
            throw new PersistenceServiceException("Unknown error when fetching Sensors! Error: "+e.getLocalizedMessage(), e);
        }
        return result;
    }
}
