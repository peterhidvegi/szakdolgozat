package hu.smartparking.ejbservice.domain;

import java.util.Date;

/**
 * Created by Perec on 2016.04.17..
 */
public class TicketStub {


    private final SensorStub sensorStub;
    private final Date start;

    private final Date end;
    private final String plate;


    public TicketStub(SensorStub sensorStub, Date start, Date end, String plate) {
        this.sensorStub = sensorStub;
        this.start = start;
        this.end = end;
        this.plate = plate;
    }

    public SensorStub getSensorStub() {
        return sensorStub;
    }

    public Date getStart() {
        return start;
    }

    public Date getEnd() {
        return end;
    }

    public String getPlate() {
        return plate;
    }


    @Override
    public String toString() {
        return " Ticket [ Sensor = "+this.sensorStub+", start = "+ this.start +", end = "+this.end+", plate = "+this.plate+"]";
    }
}
