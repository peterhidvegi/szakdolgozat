package hu.smartparking.ejbservice.domain;

import java.util.Date;

/**
 * Created by Perec on 2016.04.18..
 */
public class TicketInputStub {

    private final Long sensor_id;
    private final Date start;
    private final Date end;
    private final String plate;

    public TicketInputStub(){this(null,null,null,null);}

    public TicketInputStub(Long sensor_id, Date start, Date end, String plate) {
        this.sensor_id = sensor_id;
        this.start = start;
        this.end = end;
        this.plate = plate;

    }

    public Long getSensor_id() {
        return sensor_id;
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
        return "TicketInputStub [sensorId="+this.getSensor_id()+", start="+this.getStart()+", end="+this.end+", plate="+this.getPlate()+"]";
    }
}
