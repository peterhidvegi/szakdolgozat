package hu.smartparking.persistence.entity;

/**
 * Created by Perec on 2016.04.10..
 */

import hu.smartparking.persistence.enums.Status;
import hu.smartparking.persistence.parameter.SensorParameter;
import hu.smartparking.persistence.query.SensorQuery;

import java.io.Serializable;
import java.util.HashSet;
import java.util.Set;
import javax.persistence.*;


@Entity
@Table(name = "sensor")
@NamedQueries(value = { //
        @NamedQuery(name = SensorQuery.GET_BY_ID, query = "SELECT s FROM Sensor s WHERE s.id=:" + SensorParameter.ID),
        @NamedQuery(name = SensorQuery.GET_ALL, query = "SELECT a FROM Sensor a ORDER BY a.id")
      //  @NamedQuery(name = SensorQuery.GET_RDY, query = "SELECT b FROM Sensor b WHERE s.status=:" + SensorParameter.STATUS)
})
public class Sensor implements Serializable {

    private static final long serialVersionUID = -6461691410947537135L;

    @Id
    @Column(name = "id", nullable = false)
    private Long id;


    @Enumerated(EnumType.ORDINAL)
    @Column(name = "status", nullable = false)
    private Status status;


    public Sensor(){
    }


    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Status getStatus() {
        return status;
    }

    public void setStatus(Status status) {
        this.status = status;
    }


    @Override
    public String toString() {
        return " Sensor [ id = " + this.id + ", status = " + this.status + " ] ";
    }
}
