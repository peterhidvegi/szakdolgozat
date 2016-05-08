package hu.smartparking.webservice;

/**
 * Created by Perec on 2016.04.10..
 */


import hu.smartparking.ejbservice.domain.SensorStub;
import hu.smartparking.ejbservice.exception.AdaptorException;

import java.util.List;

import javax.ws.rs.*;

@Path("/sensor")
public interface SensorRestService {

    @GET
    @Path("/{node}")
    @Produces("application/json")
    SensorStub getSensor(@PathParam("node") long node) throws AdaptorException;

    @GET
    @Path("/list")
    @Produces("application/json")
    List<SensorStub> getAllSensors() throws AdaptorException;
}
