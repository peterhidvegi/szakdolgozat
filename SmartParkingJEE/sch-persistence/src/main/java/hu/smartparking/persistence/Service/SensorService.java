package hu.smartparking.persistence.Service;

import hu.smartparking.persistence.entity.Sensor;
import hu.smartparking.persistence.enums.Status;
import hu.smartparking.persistence.exception.PersistenceServiceException;

import javax.ejb.Local;
import javax.ejb.Remote;
import java.util.List;

/**
 * Created by Perec on 2016.04.10..
 */
@Local
public interface SensorService {

    Sensor readById(Long sensorId) throws PersistenceServiceException;

  //  List<Sensor> readByRdy(Status status) throws PersistenceServiceException;

    List<Sensor> readAll() throws PersistenceServiceException;

}
