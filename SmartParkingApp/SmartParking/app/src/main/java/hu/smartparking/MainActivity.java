package hu.smartparking;

import android.app.Activity;
import android.app.Fragment;
import android.app.FragmentTransaction;
import android.content.Intent;
import android.media.Image;
import android.os.Bundle;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import butterknife.Bind;
import butterknife.ButterKnife;
import de.greenrobot.event.EventBus;
import hu.smartparking.Events.GetNodes;
import hu.smartparking.Events.SaveDatas;
import hu.smartparking.Events.SendNodes;
import hu.smartparking.Fragments.TransactionFragment;
import hu.smartparking.Service.CommunicationService;

public class MainActivity extends Activity {

    EventBus eventBus;

    @Bind(R.id.plate_image)
    ImageView imageViewPlate;

    @Bind(R.id.ticket_image)
    ImageView imageViewTicket;

    @Bind(R.id.node_image)
    ImageView imageViewNode;

    @Bind(R.id.back_button)
    ImageView imageView_prev;

    @Bind(R.id.next_button)
    ImageView imageView_next;

    @Bind(R.id.time_image)
    ImageView imageView_time;

    TransactionFragment fragment;

    private float lastX;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        ButterKnife.bind(this);
        startService(new Intent(this, CommunicationService.class));
        eventBus = EventBus.getDefault();
        startTransaction();

        imageView_next.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                next();
            }
        });

        imageView_prev.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                prev();
            }
        });


    }

    private void startTransaction()
    {
        fragment = new TransactionFragment();
        FragmentTransaction transaction = getFragmentManager().beginTransaction();
        transaction.replace(R.id.fragment_container, fragment);
        transaction.commit();
    }

    @Override
    public boolean onTouchEvent(MotionEvent touchevent) {

        switch (touchevent.getAction())
        {
            case MotionEvent.ACTION_DOWN:
            {
                lastX = touchevent.getX();
                break;
            }
            case MotionEvent.ACTION_UP:
            {
                float currentX = touchevent.getX();

                if (lastX < currentX)
                {
                    prev();
                }

                if (lastX > currentX)
                {
                    next();
                }

                break;
            }
        }
        return false;
    }

    private void next()
    {
        fragment.getViewFlipper().setInAnimation(this, R.anim.in_from_right);
        fragment.getViewFlipper().setOutAnimation(this, R.anim.out_to_left);
        fragment.getViewFlipper().showNext();
        switchFlipper();
    }

    private void prev()
    {
        fragment.getViewFlipper().setInAnimation(this, R.anim.in_from_left);
        fragment.getViewFlipper().setOutAnimation(this, R.anim.out_to_right);
        fragment.getViewFlipper().showPrevious();
        switchFlipper();
    }

    private void switchFlipper()
    {
        switch (fragment.getViewFlipper().getDisplayedChild())
        {
            case 0:

                imageViewPlate.setBackgroundResource(R.drawable.image_border);
                imageViewNode.setBackgroundResource(0);
                imageViewTicket.setBackgroundResource(0);
                imageView_time.setBackgroundResource(0);
                break;

            case 1:

                imageViewPlate.setBackgroundResource(0);
                imageViewNode.setBackgroundResource(R.drawable.image_border);
                imageViewTicket.setBackgroundResource(0);
                imageView_time.setBackgroundResource(0);
                eventBus.post(new GetNodes());
                break;

            case 2 :

                imageView_time.setBackgroundResource(R.drawable.image_border);
                imageViewPlate.setBackgroundResource(0);
                imageViewNode.setBackgroundResource(0);
                imageViewTicket.setBackgroundResource(0);
                break;

            case 3 :

                imageViewPlate.setBackgroundResource(0);
                imageViewNode.setBackgroundResource(0);
                imageView_time.setBackgroundResource(0);
                imageViewTicket.setBackgroundResource(R.drawable.image_border);
                eventBus.post(new SaveDatas());
                break;

        }
    }

}
