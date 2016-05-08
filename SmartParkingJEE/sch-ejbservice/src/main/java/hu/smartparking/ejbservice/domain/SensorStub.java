package hu.smartparking.ejbservice.domain;

/**
 * Created by Perec on 2016.04.10..
 */
public class SensorStub {

    private final StatusStub status;

    public SensorStub(StatusStub status) {
        this.status = status;
    }

    public StatusStub getStatus() {
        return status;
    }

    @Override
    public String toString() {
        return "SensorStub [ status = " + this.status+ "]";
    }
}
