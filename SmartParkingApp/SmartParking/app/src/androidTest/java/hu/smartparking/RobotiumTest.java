package hu.smartparking;

/**
 * Created by Perec on 2016.05.12..
 */

import android.test.ActivityInstrumentationTestCase2;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TimePicker;

import com.robotium.solo.Solo;

import java.util.List;

public class RobotiumTest extends ActivityInstrumentationTestCase2<MainActivity> {

    private Solo solo;


    public RobotiumTest() {
        super(MainActivity.class);
    }

    @Override
    protected void tearDown() throws Exception {
        solo.finishOpenedActivities();
    }

    @Override
    protected void setUp() throws Exception {
        solo = new Solo(getInstrumentation(),getActivity());
    }

    /**
     * Create a test ticket:
     *  plate number: "TES-566"
     *  sensor id: 2
     *  end of parking time: 13:30
     *  and click on save button.
     * @throws Exception
     */

    public void testCreateTicket() throws Exception {
        solo.assertCurrentActivity("Activity", MainActivity.class);
        solo.enterText((EditText) solo.getView(R.id.plate_text), "TES-566");
        View btnPrev = solo.getView(R.id.next_button);
        solo.clickOnView(btnPrev);
        ListView listView = (ListView) solo.getView(R.id.nodes_containers);
        solo.waitForText("2");
        View parkingPlace = listView.getChildAt(1);
        solo.clickOnView(parkingPlace);
        solo.clickOnView(btnPrev);
        TimePicker timePicker = (TimePicker) solo.getView(R.id.time_picker);
        solo.setTimePicker(timePicker, 13, 30);
        solo.clickOnView(btnPrev);
        View frame = solo.getView(R.id.fragment_container);
        View saveTicket = frame.findViewById(R.id.save_button);
        solo.clickOnView(saveTicket);
    }
}
