package hu.smartparking.ejbservice.converter;

import hu.smartparking.ejbservice.domain.SensorStub;
import hu.smartparking.ejbservice.domain.StatusStub;
import hu.smartparking.persistence.entity.Sensor;

import javax.ejb.Stateless;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Perec on 2016.04.10..
 */
@Stateless
public class SensorConverterImpl implements SensorConverter {

    @Override
    public SensorStub to(Sensor sensor) {
        final StatusStub status = StatusStub.valueOf(sensor.getStatus().toString());
        return new SensorStub(status);
    }

    @Override
    public List<SensorStub> to(List<Sensor> sensors) {
        final List<SensorStub> stubs = new ArrayList<>();
        for (final Sensor sensor : sensors) {
            stubs.add(this.to(sensor));
        }
        return stubs;
    }
}
