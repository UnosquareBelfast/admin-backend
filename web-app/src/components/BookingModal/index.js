import React from 'react';
import container from './container';
import ModalStatusBanner from './ModalStatusBanner';
import { PropTypes as PT } from 'prop-types';
import BookingModalForm from './BookingModalForm';
import { Modal } from '../common';
import { StyleContainer, FormContainer } from './styled';
import { Spinner } from '../common';
import { SpinnerContainer } from '../../hoc/AuthUserAndStore/styled';
import LegacyMessageList from './LegacyMessageList';

const rejectionReason = booking => {
  if (booking.messages) {
    return booking.messages.message;
  }
  return undefined;
};

const BookingModal = props => {
  const {
    booking,
    employeeId,
    closeBookingModal,
    bookingModalOpen,
    updateTakenEvents,
    isEventBeingUpdated,
    bookingDuration,
    createEvent,
    updateEvent,
    cancelEvent,
    toggleRejectionMessageInputView,
    toggleRejectionResponseView,
    toggleLegacyHolidayMessageView,
    toggleRejectionMessageView,
    loading,
  } = props;

  const renderSpinner = () => {
    return (
      <SpinnerContainer>
        <Spinner />
      </SpinnerContainer>
    );
  };

  const renderLegacyMesasge = () => {
    if (toggleRejectionMessageView) {
      return <LegacyMessageList />;
    }
    return null;
  };

  const renderBookingModalForm = () => {
    if (!toggleRejectionMessageView) {
      return (<div><h1>
        {isEventBeingUpdated ? 'Update Booking' : 'Request a Booking'}
      </h1>
        <h4 id="totalDaysToBook">Total days: {bookingDuration}</h4>
        <FormContainer>
          <BookingModalForm
            toggleRejectionMessageView={toggleRejectionMessageView}
            toggleRejectionResponseView={toggleRejectionResponseView}
            updateTakenEvents={updateTakenEvents}
            employeeId={employeeId}
            booking={booking}
            bookingDuration={bookingDuration}
            createEvent={createEvent}
            updateEvent={updateEvent}
          />
        </FormContainer> </div>);
    }
    return null;
  };

  const renderModalContent = () => {
    return (
      <StyleContainer>
        {isEventBeingUpdated && (
          <ModalStatusBanner
            toggleRejectionMessageView={toggleRejectionMessageView}
            toggleLegacyHolidayMessageView={toggleLegacyHolidayMessageView}
            toggleRejectionResponseView={toggleRejectionResponseView}
            userName={booking.title}
            eventStatus={booking.eventStatus}
            eventType={booking.eventType}
            cancelEvent={cancelEvent}
            rejectionReason={rejectionReason(booking)}
            toggleRejectionMessageInputView={toggleRejectionMessageInputView}
          />
        )}
        {renderLegacyMesasge()}
        {renderBookingModalForm()}
      </StyleContainer>
    );
  };

  return (
    bookingModalOpen && (
      <Modal closeModal={closeBookingModal}>
        {!loading ? renderModalContent() : renderSpinner()}
      </Modal>
    ));
};

BookingModal.propTypes = {
  booking: PT.object.isRequired,
  employeeId: PT.number,
  closeBookingModal: PT.func.isRequired,
  bookingModalOpen: PT.bool,
  updateTakenEvents: PT.func.isRequired,
  bookingDuration: PT.number,
  createEvent: PT.func.isRequired,
  updateEvent: PT.func.isRequired,
  cancelEvent: PT.func.isRequired,
};

export default container(BookingModal);
