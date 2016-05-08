package hu.smartparking.persistence.exception;

/**
 * Created by Perec on 2016.04.10..
 */
public class PersistenceServiceException extends Exception{

    private static final long serialVersionUID = 1207428295818535206L;

    public PersistenceServiceException(String message, Throwable cause) {
        super(message, cause);
    }
}
