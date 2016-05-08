package hu.smartparking.Entities;

import hu.smartparking.Enum.Status;

/**
 * Created by Perec on 2016.04.22..
 */
public class Sensor {
    public int getNode() {
        return node;
    }

    private int node;

    public Status getStatus() {
        return status;
    }

    private Status status;

    public Sensor(int node, Status status) {
        this.node = node;
        this.status = status;
    }
}
