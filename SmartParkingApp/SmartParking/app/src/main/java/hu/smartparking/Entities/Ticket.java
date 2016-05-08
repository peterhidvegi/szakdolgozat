package hu.smartparking.Entities;

import java.util.Date;

/**
 * Created by Perec on 2016.04.22..
 */
public class Ticket {

    public int getNode() {
        return node;
    }

    public void setNode(int node) {
        this.node = node;
    }

    public String getPlate() {
        return plate;
    }

    public void setPlate(String plate) {
        this.plate = plate;
    }

    public Date getDate() {
        return date;
    }

    public void setDate(Date date) {
        this.date = date;
    }

    public Date getEnd() {
        return end;
    }

    public void setEnd(Date end) {
        this.end = end;
    }


    public Ticket(int node, String plate, Date date, Date end) {
        this.node = node;
        this.plate = plate;
        this.date = date;
        this.end = end;
    }

    int node;
    String plate;
    Date date;
    Date end;

}
