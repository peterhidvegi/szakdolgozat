package hu.smartparking.Rest;

import android.content.Context;

import com.android.volley.RequestQueue;
import com.android.volley.toolbox.HurlStack;
import com.android.volley.toolbox.Volley;

import java.io.IOException;
import java.net.HttpURLConnection;
import java.net.URL;

/**
 * Created by Perec on 2016.04.15..
 */
public class VolleySingleton {

    private static VolleySingleton instance;
    private RequestQueue requestQueue;
    private Context context;

    private VolleySingleton(Context context){
        this.context = context;
        requestQueue = newRequestQueue(true);
    }

    public static VolleySingleton getInstance(Context context) {
        if (instance == null) {
            instance = new VolleySingleton(context);
        }
        return instance;
    }

    public RequestQueue getRequestQueue() {
        return requestQueue;
    }

    public RequestQueue newRequestQueue(boolean followRedirect){
        if(followRedirect){
            requestQueue = Volley.newRequestQueue(context);
        } else {
            requestQueue = Volley.newRequestQueue(context, new HurlStack() {
                @Override
                protected HttpURLConnection createConnection(URL url) throws IOException {
                    HttpURLConnection connection = super.createConnection(url);
                    connection.setInstanceFollowRedirects(false);
                    return connection;
                }
            });
        }
        return requestQueue;
    }

}
