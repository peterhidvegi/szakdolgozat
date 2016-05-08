package hu.smartparking.webservice;

import hu.smartparking.ejbservice.domain.SensorStub;
import hu.smartparking.ejbservice.domain.StatusStub;
import hu.smartparking.ejbservice.exception.AdaptorException;
import hu.smartparking.ejbservice.facade.SensorFacade;
import hu.smartparking.persistence.enums.Status;

import javax.ejb.EJB;
import javax.ejb.Stateless;
import java.util.List;

/**
 * Created by Perec on 2016.04.10..
 */
@Stateless
public class SensorsRestServiceBean implements SensorRestService {

    @EJB
    private SensorFacade facade;

    @Override
    public SensorStub getSensor(long id) throws AdaptorException {
        return this.facade.getSensor(id);
    }

    @Override
    public List<SensorStub> getAllSensors() throws AdaptorException {
        return this.facade.getAllSensors();
    }
}
