package hu.smartparking.Events;

import java.util.List;

import hu.smartparking.Entities.Sensor;

/**
 * Created by Perec on 2016.04.22..
 */
public class SendNodes {

    public List<Sensor> getSensors() {
        return sensors;
    }

    private List<Sensor> sensors;

    public SendNodes(List<Sensor> sensors) {
        this.sensors = sensors;
    }
}
