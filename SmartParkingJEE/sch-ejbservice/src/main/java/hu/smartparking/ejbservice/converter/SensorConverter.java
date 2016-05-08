package hu.smartparking.ejbservice.converter;

/**
 * Created by Perec on 2016.04.10..
 */

import hu.smartparking.ejbservice.domain.SensorStub;
import hu.smartparking.persistence.entity.Sensor;

import javax.ejb.Local;
import javax.ejb.Remote;
import java.util.List;


@Local
public interface SensorConverter {

    SensorStub to(Sensor sensor);

    List<SensorStub> to(List<Sensor> sensors);
}
