package hu.smartparking.Fragments;

import android.app.Fragment;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.TimePicker;
import android.widget.ViewFlipper;

import java.util.Calendar;
import java.util.List;

import de.greenrobot.event.EventBus;
import hu.smartparking.Entities.Sensor;
import hu.smartparking.Entities.Ticket;
import hu.smartparking.Events.SaveDatas;
import hu.smartparking.Events.SendNodes;
import hu.smartparking.R;

/**
 * Created by Perec on 2016.04.20..
 */
public class TransactionFragment extends Fragment{

    View view;

    public ViewFlipper getViewFlipper() {
        return viewFlipper;
    }
    Calendar endTime;
    ViewFlipper viewFlipper;
    ListView listView;
    EditText plateText;
    TextView textSavePlate;
    TextView choosenNode;
    TextView saveNode;
    TextView savePlate;
    TextView saveDate;
    TextView saveEndDate;
    Button saveButton;
    TimePicker timePicker;
    List<Sensor> sensors;
    EventBus eventBus;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment, container, false);
        eventBus = EventBus.getDefault();
        if(!eventBus.isRegistered(this))
            eventBus.register(this);

        viewFlipper = (ViewFlipper) view.findViewById(R.id.view_flipper);
        plateText = (EditText) view.findViewById(R.id.plate_text);
        textSavePlate = (TextView) view.findViewById(R.id.save_plate);
        choosenNode = (TextView) view.findViewById(R.id.choosen_node);
        saveNode = (TextView) view.findViewById(R.id.save_node);
        savePlate = (TextView) view.findViewById(R.id.save_plate);
        saveDate = (TextView) view.findViewById(R.id.save_date);
        saveButton = (Button) view.findViewById(R.id.save_button);
        saveEndDate = (TextView) view.findViewById(R.id.save_end_date);
        timePicker = (TimePicker) view.findViewById(R.id.time_picker);
        listView = (ListView) view.findViewById(R.id.nodes_containers);
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                choosenNode.setText(((TextView) view.findViewById(R.id.element_text_view)).getText());
            }
        });
        saveButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                eventBus.post(new Ticket(Integer.parseInt(saveNode.getText().toString()), savePlate.getText().toString(), Calendar.getInstance().getTime(), endTime.getTime()));
            }
        });
        timePicker.setIs24HourView(true);
        return view;
    }


    public void onEvent(SendNodes sendNodes)
    {
        sensors = sendNodes.getSensors();
        SensorListViewFragment adapter = new SensorListViewFragment(getActivity(),sensors);
        listView.setAdapter(adapter);
    }

    public void onEvent(SaveDatas saveDatas)
    {
        textSavePlate.setText(plateText.getText().toString());
        saveNode.setText(choosenNode.getText().toString());
        saveDate.setText(Calendar.getInstance().getTime().toString());
        timePicker.clearFocus();
        endTime = Calendar.getInstance();
        endTime.set(Calendar.HOUR_OF_DAY,timePicker.getCurrentHour());
        endTime.set(Calendar.MINUTE,timePicker.getCurrentMinute());
        saveEndDate.setText(endTime.getTime().toString());

    }

}
