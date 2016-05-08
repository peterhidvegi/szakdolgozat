package hu.smartparking.ejbservice.facade;

import hu.smartparking.ejbservice.domain.SensorStub;
import hu.smartparking.ejbservice.domain.StatusStub;
import hu.smartparking.ejbservice.exception.AdaptorException;
import hu.smartparking.persistence.enums.Status;

import javax.ejb.Local;
import javax.ejb.Remote;
import java.util.List;

/**
 * Created by Perec on 2016.04.10..
 */

@Local
public interface SensorFacade {

    SensorStub getSensor(Long id) throws AdaptorException;

  //  List<SensorStub> getSensorsByStatus(Status status) throws AdaptorException;

    List<SensorStub> getAllSensors() throws AdaptorException;
}
