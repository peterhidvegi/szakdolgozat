package hu.smartparking.persistence.entity;

/**
 * Created by Perec on 2016.04.17..
 */

import javax.persistence.*;
import java.io.Serializable;
import java.util.Date;


@Entity
@Table(name = "ticket")
public class Ticket implements Serializable {

    private static final long serialVersionUID = -6461691410947537134L;

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name="id", unique=true, nullable = false)
    private Long id;

    @OneToOne(fetch = FetchType.LAZY, cascade = CascadeType.ALL, optional = false)
    @JoinColumn(name = "sensor_id", referencedColumnName = "id", nullable = false)
    private Sensor sensor;

    @Temporal(TemporalType.TIMESTAMP)
    @Column(name="start", nullable = false)
    private Date start;

    @Temporal(TemporalType.TIMESTAMP)
    @Column(name="end", nullable = false)
    private Date end;

    @Column(name = "plate")
    private String plate;

    public Ticket() {
    }

    public Ticket(Sensor sensor, Date start, Date end, String plate) {
        this.sensor = sensor;
        this.start = start;
        this.end = end;
        this.plate = plate;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Date getStart() {
        return start;
    }

    public void setStart(Date start) {
        this.start = start;
    }

    public Date getEnd() {
        return end;
    }

    public void setEnd(Date end) {
        this.end = end;
    }

    public String getPlate() {
        return plate;
    }

    public void setPlate(String plate) {
        this.plate = plate;
    }

    public Sensor getSensor() {
        return sensor;
    }

    public void setSensor(Sensor sensor) {
        this.sensor = sensor;
    }


    @Override
    public String toString() {
        return " Ticket [ id = " + this.id + ", Sensor = "+this.sensor+", start = "+ this.start +", end = "+this.end+", plate = "+this.plate+"]";
    }
}
