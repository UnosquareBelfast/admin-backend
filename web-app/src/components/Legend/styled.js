import styled from 'styled-components';

export const StyleContainer = styled.div`
  box-sizing: border-box;
  background-color: ${props => props.theme.colours.lightgrey};
  border: 1px solid ${props => props.theme.colours.grey};
  border-radius: 0 0 6px 6px;
  border-top: none;
  width: 100%;
  padding: 20px 10px 0 10px;
  display: block;
  @media (min-width: 1220px) {
    display: flex;
  }

  svg {
    margin-right: 10px;
  }
`;

export const Column = styled.div`
    width: 100%;
    margin: 10px;
  }

  h4 {
    width: 100%;
    margin: 0 0 10px 0;
  }
`;

export const Key = styled.div`
  box-sizing: border-box;
  background-color: white;
  background-color: ${props => props.theme.holidayStatus[props.status]};
  border: 2px solid ${props => props.theme.holidayStatus[props.status]};
  color: white;
  font-size: 12px;
  font-weight: 400;
  margin: 0px 4px 4px 0;
  border-radius: 3px;
  padding: 8px;
  cursor: pointer;
  transition: all 150ms;
  &:hover {
    opacity: 0.9;
  }

  &.selected {
    box-shadow: inset 0 0 0 2px white;
  }
`;
