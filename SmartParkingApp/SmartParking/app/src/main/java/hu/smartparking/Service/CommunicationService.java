package hu.smartparking.Service;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.widget.Toast;

import com.android.volley.AuthFailureError;
import com.android.volley.NetworkResponse;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import de.greenrobot.event.EventBus;
import hu.smartparking.Constants.Addresses;
import hu.smartparking.Entities.Sensor;
import hu.smartparking.Entities.Ticket;
import hu.smartparking.Enum.Status;
import hu.smartparking.Events.GetNodes;
import hu.smartparking.Events.SendNodes;
import hu.smartparking.Rest.VolleySingleton;

/**
 * Created by Perec on 2016.04.20..
 */
public class CommunicationService extends Service {

    private EventBus eventBus;

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {

        eventBus = EventBus.getDefault();

        if(!eventBus.isRegistered(this))
            eventBus.register(this);

        return START_STICKY;
    }

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    public void onEvent(final Ticket getTicket)
    {
        Map<String, String>  params = new HashMap<String, String> ();
        params.put("sensor_id", String.valueOf(getTicket.getNode()));
        params.put("start", String.valueOf(getTicket.getDate().getTime()));
        params.put("end", String.valueOf(getTicket.getEnd().getTime()));
        params.put("plate", getTicket.getPlate());

        RequestQueue requestQueue = VolleySingleton.getInstance(CommunicationService.this).getRequestQueue();
        JsonObjectRequest request = new JsonObjectRequest(Request.Method.PUT, Addresses.TO_SEND_TICKET, new JSONObject(params), new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonObject) {

            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Toast.makeText(CommunicationService.this, error.getMessage(), Toast.LENGTH_LONG).show();
            }

        })
        {
            @Override
            protected Response<JSONObject> parseNetworkResponse(NetworkResponse response) {
                Toast.makeText(CommunicationService.this,response.statusCode,Toast.LENGTH_LONG).show();
                return super.parseNetworkResponse(response);
            }

            @Override
            protected VolleyError parseNetworkError(VolleyError volleyError) {
                Toast.makeText(CommunicationService.this,volleyError.networkResponse.statusCode,Toast.LENGTH_LONG).show();
                return super.parseNetworkError(volleyError);
            }

            @Override
            public Map<String, String> getHeaders() throws AuthFailureError {
                return generateHeaders();
            }
        };
        requestQueue.add(request);
    }

    public void onEvent(GetNodes getNodes)
    {
        RequestQueue requestQueue = VolleySingleton.getInstance(CommunicationService.this).getRequestQueue();
        StringRequest stringRequest = new StringRequest(Request.Method.GET, Addresses.TO_SEND_PLATE,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        try {
                            List<Sensor> sensors = new ArrayList<Sensor>();
                            JSONArray nodesArray = new JSONArray(response);
                            for (int i = 0; i < nodesArray.length();i++)
                            {
                                if(nodesArray.getJSONObject(i).getString("status").equals(Status.READY.toString()))
                                    sensors.add(new Sensor(i+1,Status.READY));
                                else
                                    sensors.add(new Sensor(i+1, Status.NOTREADY));
                            }
                            eventBus.post(new SendNodes(sensors));
                        } catch (JSONException e) {
                            e.printStackTrace();
                            Toast.makeText(CommunicationService.this,e.getMessage(),Toast.LENGTH_LONG).show();
                        }
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Toast.makeText(CommunicationService.this,error.getMessage(),Toast.LENGTH_LONG).show();
            }
        })
        {
            @Override
            public Map<String, String> getHeaders() throws AuthFailureError {
                return generateHeaders();
            }
        };
        requestQueue.add(stringRequest);
    }

    private HashMap<String, String> generateHeaders() {

        HashMap<String, String> headers = new HashMap<>();
        headers.put("Content-Type", "application/json; charset=utf-8");
        headers.put("accept", "*/*");
        headers.put("accept-encoding", "");
        return headers;
    }
}
