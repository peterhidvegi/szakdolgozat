package hu.smartparking.ejbservice.facade;

import hu.smartparking.ejbservice.converter.SensorConverter;
import hu.smartparking.ejbservice.domain.SensorStub;
import hu.smartparking.ejbservice.domain.StatusStub;
import hu.smartparking.ejbservice.exception.AdaptorException;
import hu.smartparking.ejbservice.util.ApplicationError;
import hu.smartparking.persistence.Service.SensorService;
import hu.smartparking.persistence.enums.Status;
import hu.smartparking.persistence.exception.PersistenceServiceException;

import javax.ejb.EJB;
import javax.ejb.Stateless;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Perec on 2016.04.10..
 */
@Stateless(mappedName = "ejb/sensorFacade")
public class SensorFacadeImpl implements SensorFacade {

    @EJB
    private SensorService sensorService;

    @EJB
    private SensorConverter converter;


    @Override
    public SensorStub getSensor(Long id) throws AdaptorException {
        SensorStub sensorStub = null;

        try{
            sensorStub = this.converter.to(this.sensorService.readById(id));
        }catch (final PersistenceServiceException e)
        {
            throw new AdaptorException(ApplicationError.UNEXPECTED, e.getLocalizedMessage());
        }

        return  sensorStub;
    }
/*
    @Override
    public List<SensorStub> getSensorsByStatus(Status status) throws AdaptorException {

        List<SensorStub> stubs = new ArrayList<>();
        try {
            stubs = this.converter.to(this.sensorService.readByRdy(status));
        } catch (final PersistenceServiceException e) {
            throw new AdaptorException(ApplicationError.UNEXPECTED, e.getLocalizedMessage());
        }
        return stubs;
    }
*/
    @Override
    public List<SensorStub> getAllSensors() throws AdaptorException {
        List<SensorStub> stubs = new ArrayList<>();
        try {
            stubs = this.converter.to(this.sensorService.readAll());
        } catch (final PersistenceServiceException e) {
            throw new AdaptorException(ApplicationError.UNEXPECTED, e.getLocalizedMessage());
        }
        return stubs;
    }
}
