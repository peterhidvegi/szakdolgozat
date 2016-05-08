package hu.smartparking.Fragments;

import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.List;

import hu.smartparking.Entities.Sensor;
import hu.smartparking.Enum.Status;
import hu.smartparking.R;

/**
 * Created by Perec on 2016.04.22..
 */
public class SensorListViewFragment extends ArrayAdapter<Sensor>{

    private static class ViewHolder{
        TextView node;
        ImageView occupancy;
    }

    public SensorListViewFragment(Context context, List<Sensor> objects) {
        super(context, R.layout.list_element, objects);
    }

    public View getView(int position, View convertView, ViewGroup parent) {

        Sensor sensor = getItem(position);

        ViewHolder viewHolder;

        if (convertView == null) {
            viewHolder = new ViewHolder();
            LayoutInflater inflater = LayoutInflater.from(getContext());
            convertView = inflater.inflate(R.layout.list_element, parent, false);
            viewHolder.node = (TextView) convertView.findViewById(R.id.element_text_view);
            viewHolder.occupancy = (ImageView) convertView.findViewById(R.id.element_image_view);
            convertView.setTag(viewHolder);
        }else
        {
            viewHolder = (ViewHolder) convertView.getTag();
        }

        viewHolder.node.setText(Integer.toString(sensor.getNode()));
        if(sensor.getStatus() == Status.NOTREADY)
        {
            viewHolder.occupancy.setBackgroundColor(Color.RED);
        }else{
            viewHolder.occupancy.setBackgroundColor(Color.GREEN);
        }

        return convertView;
    }
}
